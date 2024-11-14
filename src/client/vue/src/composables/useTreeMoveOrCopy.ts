import { h, ref } from "vue";
import { useDialog } from "./useDialog";
import { capitalize, i18n, logger } from "~/libs";
import {  ElMessageBox } from "element-plus";

export function useTreeMoveOrCopy(
    api: any,
    config: any = {}
) {
    const target = ref<any>();
    const currentMode = ref<'move' | 'copy' | null>(null);
    const submit = async () => {
        if(!target.value) {
            dialogRef.value?.error("Components.NoSelectedNodeToMoveOrCopy");
            return;
        };        
        if(currentMode.value === null) return;
        const confirm = await confirmMoveOrCopy();
        if(!confirm) return;
        try {
            const method = currentMode.value ==='move'? api.move : api.copy;
            await method(dialogData.value[api.idFieldName], target.value[api.idFieldName]);
            dialogRef.value?.success("Message.SaveSuccessAndClose");
            setTimeout(() => {
                dialogData.value = null;
                dialogVisible.value = false;
                config.moveOrCopyRefresh && config.moveOrCopyRefresh(currentMode.value, dialogData.value, target.value);
            }, 3000);
        } catch (e: any) {
            dialogRef.value.error(e.message);
            logger.error("[useTreeMoveOrCopy][okClick]", e);
        }
    };
    
    const checkChange = ()=>false;

    const confirmMoveOrCopy = async () => {
        const actionMessage = i18n.get('Components.' + capitalize(currentMode.value || ''));
        return await ElMessageBox.confirm(
            i18n.get('Message.ConfirmMoveOrCopyMessage', { 
                action: `<span class="text-primary">${actionMessage}</span>`,
                source: `<span class="text-error">${dialogData.value[api.messageField]}</span>`,
                target: `<span class="text-success">${target.value[api.messageField]}</span>`
            }),
            i18n.get('Message.ConfirmMoveOrCopyTitle', { action: actionMessage }),
            {
                dangerouslyUseHTMLString: true,
            }
        );
    }

    const {
        dialogVisible,
        dialogTitle,
        dialogTitlePrefix,
        dialogData,
        dialogProps,
        dialogRef
    } = useDialog({
        submit,
        checkChange,
        ...config,
    });

    const showMoveDialog = (row: any) => {
        currentMode.value ='move';
        dialogTitlePrefix.value = "Components.SelectedMoveTarget";
        showDialog(row);
    };
    const showCopyDialog = (row: any) => {
        currentMode.value ='copy';
        dialogTitlePrefix.value = "Components.SelectedCopyTarget";
        showDialog(row);
    };    

    const showDialog = async (row: any) => {
        dialogData.value = row;
        dialogTitle.value = row[api.messageField];
        dialogVisible.value = true;
    };

    const buttons = ()=>{
        return {
            move:{
                action: showMoveDialog,
                type: 'primary',
                icon: 'fa fa-arrows',
                title: 'Components.MoveNode',
                order: 250,
                visible: api.canUpdate,
            },
            copy:{
                action: showCopyDialog,
                type: 'primary',
                icon: 'fa fa-copy',
                title: 'Components.CopyNode',
                order: 255,
                visible: api.canUpdate,
            }
        }
    };

    return {
        treeMoveOrCopyDialogRef: dialogRef,
        treeMoveOrCopyDialogVisible: dialogVisible,
        treeMoveOrCopySource: dialogData,
        treeMoveOrCopyTarget: target,
        treeMoveOrCopyDialogProps: dialogProps,
        treeMoveOrCopyButtons: buttons()
    };
}
