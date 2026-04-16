import * as z from 'zod';


export interface CreateItemRequestDto {
    name: string;
    description? : string;
}

export const CreateItemRequestValidators = {
    name : z.string()
        .nonempty({
            error: 'Name is required'
        })
        .min(10,{
            error: 'At least 10 characters'
        })
};

export const CreateItemRequestDtoSchema = z.object({
    name: CreateItemRequestValidators.name,
    description: z.string().optional()
});
