export type SelectItem<T extends string | number | boolean> = {
    label: string;
    value: T;
}