import { makeAutoObservable, runInAction } from "mobx"
import agent from "../api/agent";
import axios from "axios";
import { store } from "./store";
import { string } from "yup";

export default class PaymentStore {
    redicrectUrl = '';
    verificationCode = '';

    constructor() {
        makeAutoObservable(this);
    }

    createPaymnet = async (amount: number) => {
        try {
            const redicrectUrl = await agent.Payments.post(amount);
            runInAction(() => {
                this.redicrectUrl = redicrectUrl;
            })
            window.open(this.redicrectUrl, '_blank');

        } catch (error) {
            console.log(error);
        }
    }

    verifyPayment = async (requestUrl: string) => {
        try {
            await axios
                .get(`http://localhost:5000/api/payment/${store.userStore.user?.userName}` + requestUrl)
                .then(response => {
                    runInAction(() => this.verificationCode = response.data)
                })
            return this.verificationCode;
        } catch (error) {
            console.log(error);
        }
    }
}