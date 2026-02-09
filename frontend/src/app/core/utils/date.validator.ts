import {
    AbstractControl,
    ValidationErrors
} from "@angular/forms";

export function dateRangeValidator(
    min: number,
    max: number
) {
    return (control: AbstractControl): ValidationErrors | null => {
        if (!control.value) return null;
        const dateYear = new Date(control.value).getFullYear();
        const nowYear = new Date().getFullYear();
        if (nowYear - dateYear > max) return { dateRange: true };
        if (nowYear - dateYear < min) return { dateRange: true };
        return null;
    };
}