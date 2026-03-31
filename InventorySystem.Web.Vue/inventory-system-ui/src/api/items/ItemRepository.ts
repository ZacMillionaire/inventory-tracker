import ky from 'ky'

interface CreateItemRequestDto {
    name: string;
    description? : string;
}

export function ItemRepository() {

    const apiUrl = import.meta.env.VITE_API_URL;
    async function GetItems(){
        await ky.get(`${apiUrl}/items`).json()
            .then(res => {
                console.log(res)
            });
    }

    async function CreateItem(newItem : CreateItemRequestDto){
        await ky.post(`${apiUrl}/items/create`,{
            json: newItem
        })
            .json()
            .then(res => {
                console.log('create response:',res);
            })
    }

    return { GetItems, CreateItem }
};

export type { CreateItemRequestDto };
