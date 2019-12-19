import { Post } from '.';

export class Person {
    public personId: any;
    public userId: any;
    public username: string;
    public name: string;
    public description: string;
    public email: string;
    public isFollowing: boolean;
    public following: Person[];
    public followingCount: number;
    public followers: Person[];
    public followersCount: boolean;
    public posts: Post[];
    public postsCount: boolean;
    public profilePicture: string;
}
