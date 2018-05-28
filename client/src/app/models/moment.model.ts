export interface Moment {
    topic: string;
    recordDate: Date;
    content?: string;
}

export interface GroupedMoments {
    group: string;
    moments: Moment[];
}