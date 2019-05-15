import { Person } from './person.model';

export class Comment {
    public commentId: string;
    public text: string;
    public commentReply: Comment;
    public person: Person;
}
