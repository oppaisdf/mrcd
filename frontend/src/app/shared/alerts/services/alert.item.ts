export type ConfirmationAction = () => void | Promise<unknown>;

export type AlertItem = {
    type: 'error' | 'success' | 'loading';
    title?: string;
    message: string;
} | {
    type: 'confirmation';
    title: string;
    message: string;
    onConfirm: ConfirmationAction;
};
