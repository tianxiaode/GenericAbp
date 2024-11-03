import { ref } from "vue";
import { RowItemType } from "~/components/Detail.vue";
import { capitalize, Repository } from "~/libs";

// interface DetailButton {
//     action: (row: any) => void;
//     icon: string;
//     title: string;
//     order: number;
// }
type OptionalLabelRowItemType = Omit<RowItemType, 'label'> & Partial<Pick<RowItemType, 'label'>>;

export function useDetail(api:Repository<any>, rows: OptionalLabelRowItemType[], useIdRow?: boolean, useDefaultRows?: boolean ) {
    const detailVisible = ref(false);
    const detailData = ref<any>({});
    const detailTitle = ref("");
    const defaultRowItems = () : RowItemType[]=> {
        return [
            { field: 'creationTime', label: 'CreationTime', type: 'datetime' },
            { field: 'creatorId', label: 'CreatorId' },
            { field: 'lastModificationTime', label: 'LastModificationTime', type: 'datetime' },
            { field: 'lastModifierId', label: 'LastModifierId' },        
        ];
    }
    const showDetails = (row: any) => {
        const id = row[api.idFieldName];
        api.getEntity(id).then((data) => {
            detailData.value = data;
            detailVisible.value = true;
            detailTitle.value = data[api.messageField];
        }).catch((e:any) => {
            //ElMessage.error(e.message);
        })
    };


    const rowItems = ():RowItemType[] =>{
        const items: RowItemType[] = [];
        if(useIdRow !== false){
            items.push({ label: 'ID', field: api.idFieldName });
        }
        rows.forEach(row => {
            items.push({
                label:`${api.resourceName}.${capitalize(api.LabelPrefix)}:${capitalize(row.field)}`,
                ...row
            });
        });
        return useDefaultRows !== false ? [...items,...defaultRowItems()] : items;
    }


    return {
        detailVisible,
        detailData,
        detailTitle,
        showDetails,
        rowItems: rowItems()
    };
}
