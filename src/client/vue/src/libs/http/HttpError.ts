export class HttpError extends Error {
    status: number;
    validationErrors: any;
    constructor(message: string, status: number,validationErrors?: any ) {
        super(message);
        this.status = status;        
        this.validationErrors = validationErrors;
    }
}