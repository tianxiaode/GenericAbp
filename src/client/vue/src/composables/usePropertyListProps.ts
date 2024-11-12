export function usePropertyListProps(defaultClassName: string = "w-9/12") {
    return {
        className: {
            type: String,
            default: defaultClassName,
        },
        labelClassName: {
            type: String,
        },
    };
}
