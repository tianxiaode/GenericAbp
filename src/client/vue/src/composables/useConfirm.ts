import { ElMessageBox } from "element-plus";
import { i18n } from "~/libs";

export function useConfirm() {
    const confirm = async (message: string, title: string, type?: string) => {
        return ElMessageBox.confirm(message, title, {
            confirmButtonText: i18n.get("AbpUi.Ok"),
            cancelButtonText: i18n.get("AbpUi.Cancel"),
            type: type || "warning" as any,
            dangerouslyUseHTMLString: true,
        })
    };

    return { confirm };
}
