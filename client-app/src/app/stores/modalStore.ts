import { makeAutoObservable } from "mobx" 
interface Modal {
    open: boolean;
    body: JSX.Element| null;
}

export default class ModalStore {
    modal : Modal = {
        open: false,
        body: null
    }

    constructor() {
        makeAutoObservable(this)
    }

    OpenModal = (content: JSX.Element) => {
        this.modal.open = true;
        this.modal.body = content; 
    }

    CloseModal = () => {
        this.modal.open = false;
        this.modal.body = null; 
    }
}