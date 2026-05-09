
import * as z from 'zod';

interface BaseDto {
    id : string;
    createdUtc : Date;
    updatedUtc? : Date;
}

const BaseDtoSchema = z.object({
    id : z.string(),
    createdUtc: z.coerce.date(),
    updatedUtc : z.coerce.date().optional().nullable()
});

export { type BaseDto, BaseDtoSchema };
