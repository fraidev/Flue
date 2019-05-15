import { Person } from './person.model';
import { Comment } from './comment.model';

export class Post {
    public postId: string;
    public person: Person;
    public text: string;
    public likes: number;
    public comments: Comment[];
}
