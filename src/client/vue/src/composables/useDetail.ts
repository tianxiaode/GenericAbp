import { ref } from "vue";
import { RowItemType } from "~/components/Detail.vue";
import { capitalize, logger, Repository } from "~/libs";

type OptionalLabelRowItemType = Omit<RowItemType, 'label'> & Partial<Pick<RowItemType, 'label'>>;

export type useDetailOptions = {
    loadAdditionalData?: boolean,
    useIdRow?: boolean, 
    useDefaultRows?: boolean
}

export function useDetail(api:Repository<any>, rows: OptionalLabelRowItemType[],  options: useDetailOptions = {}) {
    const detailVisible = ref(false);
    const detailData = ref<any>({});
    const detailTitle = ref("");
    const defaultRowItems = () : RowItemType[]=> {
        return [
            { field: 'creationTime', label: 'Components.CreationTime', type: 'datetime' },
            { field: 'creatorId', label: 'Components.CreatorId' },
            { field: 'lastModificationTime', label: 'Components.LastModificationTime', type: 'datetime' },
            { field: 'lastModifierId', label: 'Components.LastModifierId' },        
        ];
    }
    const showDetails = (row: any) => {
        loadData(row[api.idFieldName]);
    };

    const loadData = async (id:string | number) => {
        try {
            const loadAdditionalData = options.loadAdditionalData ===false ? false : true;
            let data = await api.getEntity(id, loadAdditionalData);
            detailData.value = data;
            detailVisible.value = true;
            detailTitle.value = data[api.messageField];
                
        } catch (e) {
            logger.debug('[useDetail][loadData]', e);
        }
    }


    const rowItems = ():RowItemType[] =>{
        const items: RowItemType[] = [];
        if(options.useIdRow !== false){
            items.push({ label: 'ID', field: api.idFieldName });
        }
        rows.forEach(row => {
            items.push({
                label:`${api.resourceName}.${capitalize(api.labelPrefix)}:${capitalize(row.field)}`,
                ...row
            });
        });
        return options.useDefaultRows !== false ? [...items,...defaultRowItems()] : items;
    }


    return {
        detailVisible,
        detailData,
        detailTitle,
        showDetails,
        rowItems: rowItems()
    };
}
