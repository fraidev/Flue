import { Person } from './person.model';
import { Post } from '.';

export class Comment {
    public commentId: string;
    public text: string;
    public post: Post;
    public isMyComment: boolean;
    public person: Person;
}
