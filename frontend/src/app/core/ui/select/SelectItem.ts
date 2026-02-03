export type SelectItem<T extends string | number> = {
    label: string;
    value: T;
}