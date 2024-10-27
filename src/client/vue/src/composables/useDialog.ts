export function useDialog(props: any, dialogVisible: any, messageRef?: any) {
    const dialogBeforeClose = () => {
        if (props.beforeClose) {
            props.beforeClose().then(async (allowClose: boolean) => {
                if (!allowClose) return;
                close();
            });
        } else {
            close();
        }
    };

    const close = () => {
        if (props.close) {
            props.close();
        }
        dialogVisible.value = false;
        messageRef.value.clear();
        props.afterClose && props.afterClose();
    };


    return {
        dialogBeforeClose,
        close,
    };
}
