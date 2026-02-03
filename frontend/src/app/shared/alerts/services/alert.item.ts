import { AlertType } from "./alert.type";

export type AlertItem = {
    type: AlertType;
    title?: string;
    message: string;
};