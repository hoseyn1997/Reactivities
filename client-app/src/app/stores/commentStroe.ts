import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { ChatComment } from "../models/comment";
import { makeAutoObservable, runInAction } from "mobx";
import { store } from "./store";
import { format } from "date-fns";

export default class CommentStore {
    comments: ChatComment[] = [];
    privateComments: ChatComment[] = [];
    privateCommentRegistery = new Map<number, ChatComment>();
    privateHubconnection: HubConnection | null = null;
    hubConnection: HubConnection | null = null;

    constructor() {
        makeAutoObservable(this);
    }


    get privateCommentsByusername() {
        return Array.from(this.privateCommentRegistery.values())
    }

    get groupedPrivateComments() {
        return Object.entries(
            this.privateCommentsByusername.reduce((comments, comment) => {
                comments[comment.username] = comments[comment.username] ? [...comments[comment.username], comment] : [comment];
                return comments;
            }, {} as { [key: string]: ChatComment[] })
        )
    }







    createHubConnection = (activityid: string) => {
        if (store.activityStore.selectedActivity) {
            this.hubConnection = new HubConnectionBuilder()
                .withUrl('http://localhost:5000/chat?activityId=' + activityid, {
                    accessTokenFactory: () => store.userStore.user?.token!
                })
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();
            this.hubConnection.start()
                .catch(error => console.log('Error establishing the connection : ', error));

            this.hubConnection.on("LoadComments", (comments: ChatComment[]) => {
                runInAction(() => {
                    comments.forEach(comment => {
                        comment.createdAt = new Date(comment.createdAt + 'Z');
                        this.privateCommentRegistery.set(comment.id, comment)
                    })
                    this.comments = comments
                });
            });

            this.hubConnection.on("ReceiveComment1", (comment: ChatComment) => {
                runInAction(() => {
                    comment.createdAt = new Date(comment.createdAt);
                    this.comments.unshift(comment)
                });
            })
        }
    }

    stopHubConnection = () => {
        this.hubConnection?.stop()
            .catch(error => console.log("Error Stopping Connection : ", error));
    }

    clearComments = () => {
        this.comments = [];
        this.stopHubConnection();
    }

    addComment = async (values: any) => {
        values.activityId = store.activityStore.selectedActivity?.id;
        try {
            await this.hubConnection?.invoke("SendComment", values);
        } catch (error) {
            console.log(error);
        }
    }



    //private hub connection 
    createPrivateHubConnection = (targetUsername: string) => {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/privateChat?targetUsername=' + targetUsername, {
                accessTokenFactory: () => store.userStore.user?.token!
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();
        this.hubConnection.start()
            .catch(error => console.log('Error establishing the connection : ', error));

        this.hubConnection.on("LoadPrivateComments", (privateComments: ChatComment[]) => {
            runInAction(() => {
                privateComments.forEach(privateComment => {
                    privateComment.createdAt = new Date(privateComment.createdAt + 'Z');
                    this.privateCommentRegistery.set(privateComment.id, privateComment)
                })
                this.privateComments = privateComments;
            });
        });

        this.hubConnection.on("ReceivePrivateComment", (privateComment: ChatComment) => {
            runInAction(() => {
                privateComment.createdAt = new Date(privateComment.createdAt);
                this.privateComments.unshift(privateComment)
                this.privateCommentRegistery.set(privateComment.id, privateComment)
            });
        })
    }

    clearPrivateComments = () => {
        this.privateComments = [];
        this.privateCommentRegistery.clear();
        this.stopHubConnection();
    }

    addPrivateComment = async (values: any, targetUsername: string) => {
        values.targetUsername = targetUsername;
        try {
            await this.hubConnection?.invoke("SendPrivateComment", values);
        } catch (error) {
            console.log(error);
        }
    }
}