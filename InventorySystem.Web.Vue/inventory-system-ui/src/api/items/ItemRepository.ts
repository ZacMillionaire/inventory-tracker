import ky from 'ky';
import type { CreateItemRequestDto } from './dtos/CreateItemRequestDto';
import { ItemDtoSchema, type ItemDto } from './dtos/ItemDto';
import * as z from 'zod';

export function ItemRepository() {

    const apiUrl = import.meta.env.VITE_API_URL;

    const itemDtoArraySchema = z.array(ItemDtoSchema);

    async function GetItems() : Promise<ItemDto[]> {
        return await ky.get(`${apiUrl}/items`)
            .json<ItemDto[]>()
            .then(res => {
                const validatedResult = itemDtoArraySchema.parse(res) as ItemDto[];
                return validatedResult;
            });
    }

    async function CreateItem(newItem : CreateItemRequestDto) : Promise<ItemDto> {
        return await ky.post(`${apiUrl}/items/create`,{
            json: newItem
        })
            .json<ItemDto>()
            .then(res => {
                const validatedResult = ItemDtoSchema.parse(res) as ItemDto;
                return validatedResult;
            });
    }

    return { GetItems, CreateItem }
};
