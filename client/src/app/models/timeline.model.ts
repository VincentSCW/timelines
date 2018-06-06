export enum ProtectLevel {
  public,
  protect
}

export enum PeriodGroupLevel {
  any,
  byDay,
  byMonth,
  byYear
}

export interface Timeline {
  username: string;
  title: string;
  topicKey: string;
  protectLevel: ProtectLevel;
  accessKey?: string;
  periodGroupLevel: PeriodGroupLevel;
}