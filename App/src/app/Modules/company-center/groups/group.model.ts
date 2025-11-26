export interface UserDto {
  id: number;
  userName: string;
  email?: string;
}

export interface Group {
  id?: number;
  groupName: string;
  creationDate?: Date;
  lastModified?: Date;
  createdById?: number;
  createdBy?: UserDto;        // 👈 add this
  lastModifiedById?: number;
  lastModifiedBy?: UserDto;   // 👈 add this
  isActive: boolean;
}
