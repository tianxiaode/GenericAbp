import { capitalize } from "~/libs";

export function useLabel(api: any){
    const getLabel = (label: string) => {
        return `${api.resourceName}.${api.labelPrefix}:${capitalize(label)}`;
    };

    return {
        getLabel
    }
}