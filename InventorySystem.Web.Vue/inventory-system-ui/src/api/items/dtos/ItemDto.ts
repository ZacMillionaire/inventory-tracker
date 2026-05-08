import { BaseDtoSchema, type BaseDto } from '@/api/dtos/BaseDto';
import * as z from 'zod';

interface ItemDto extends BaseDto {
    name : string;
    description? : string;
    distinct : boolean;
}

const ItemDtoSchema = BaseDtoSchema.extend({
    name: z.string(),
    description: z.string().optional(),
    distinct: z.boolean(),
});

export { type ItemDto, ItemDtoSchema}
