import { ref } from "vue";
import { useDialog } from "./useDialog";
import { capitalize, i18n, logger } from "~/libs";
import { ElMessageBox } from "element-plus";

export function useTreeMoveOrCopy(api: any, config: any = {}) {
    const target = ref<any>();
    const currentMode = ref<"move" | "copy" | null>(null);
    const submit = async () => {
        if (!validate()) return;
        if (currentMode.value === null) return;
        const confirm = await confirmMoveOrCopy();
        if (!confirm) return;
        try {
            const method = currentMode.value === "move" ? api.move : api.copy;
            await method(
                dialogData.value[api.idFieldName],
                target.value[api.idFieldName]
            );
            dialogRef.value?.success("Message.SaveSuccessAndClose");
            setTimeout(() => {
                dialogVisible.value = false;
                config.moveOrCopyRefresh &&
                    config.moveOrCopyRefresh(
                        currentMode.value,
                        dialogData.value,
                        target.value
                    );
                    dialogData.value = null;
                    target.value = null;
            }, 3000);
        } catch (e: any) {
            dialogRef.value.error(e.message);
            dialogData.value = null;
            target.value = null;
            logger.error("[useTreeMoveOrCopy][okClick]", {
                error: e.message,
                mode: currentMode.value,
                source: dialogData.value,
                target: target.value,
            });
        }
    };

    const validate = () => {
        const errorMessage = "Message.CannotMoveOrCopyToItself";

        try {
            if (!target.value || !dialogData.value) {
                dialogRef.value?.error("Message.NoSelectedNodeToMoveOrCopy");
                return false;
            }

            const targetId = target.value[api.idFieldName];
            const dialogId = dialogData.value[api.idFieldName];
            const dialogParentId = dialogData.value[api.parentIdFieldName];

            if (targetId === dialogId) {
                dialogRef.value?.error(errorMessage);
                return false;
            }

            if (targetId === dialogParentId) {
                // 目标节点就是移动节点的父节点, 不需要移动
                dialogRef.value?.error(errorMessage);
                return false;
            }

            return true;
        } catch (error) {
            logger.error("Validation error:", error);
            return false;
        }
    };

    const checkChange = () => false;

    const confirmMoveOrCopy = async () => {
        const actionMessage = i18n.get(
            "Components." + capitalize(currentMode.value || "")
        );
        return await ElMessageBox.confirm(
            i18n.get("Message.ConfirmMoveOrCopyMessage", {
                action: `<span class="text-primary">${actionMessage}</span>`,
                source: `<span class="text-error">${
                    dialogData.value[api.messageField]
                }</span>`,
                target: `<span class="text-success">${
                    target.value[api.messageField]
                }</span>`,
            }),
            i18n.get("Message.ConfirmMoveOrCopyTitle", {
                action: actionMessage,
            }),
            {
                dangerouslyUseHTMLString: true,
            }
        );
    };

    const {
        dialogVisible,
        dialogTitle,
        dialogTitlePrefix,
        dialogData,
        dialogProps,
        dialogRef,
    } = useDialog({
        submit,
        checkChange,
        ...config,
    });

    const showMoveOrCopyDialog = (mode: "move" | "copy", row: any) => {
        currentMode.value = mode;
        dialogTitlePrefix.value = `Components.Selected${capitalize(
            mode
        )}Target`;
        dialogData.value = row;
        dialogTitle.value = row[api.messageField];
        dialogVisible.value = true;
    };

    const showMoveDialog = (row: any) => showMoveOrCopyDialog("move", row);
    const showCopyDialog = (row: any) => showMoveOrCopyDialog("copy", row);

    const buttons = () => {
        return {
            move: {
                action: showMoveDialog,
                type: "primary",
                icon: "fa fa-arrows",
                title: "Components.MoveNode",
                order: 250,
                visible: api.canUpdate,
            },
            copy: {
                action: showCopyDialog,
                type: "primary",
                icon: "fa fa-copy",
                title: "Components.CopyNode",
                order: 255,
                visible: api.canUpdate,
            },
        };
    };

    return {
        treeMoveOrCopyDialogRef: dialogRef,
        treeMoveOrCopyDialogVisible: dialogVisible,
        treeMoveOrCopySource: dialogData,
        treeMoveOrCopyTarget: target,
        treeMoveOrCopyDialogProps: dialogProps,
        treeMoveOrCopyButtons: buttons(),
    };
}
