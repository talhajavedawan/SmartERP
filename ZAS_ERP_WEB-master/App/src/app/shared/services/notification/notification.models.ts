export interface Notification {
  id: number;
  title: string;
  description?: string;
  info?: string;
  transactionId: number;
  transactionType: TransactionItemType;
  userId?: number;
  sendingUserId?: number;
  ccUserId?: number;
  isRead: boolean;
  isPending: boolean;
  glow: boolean;
  timestamp: Date | string;
  readTimestamp?: Date | string;
  billReferenceNo?: string;
  poReferenceNo?: string;
  flagId?: number;
  commentLogId?: number;

  // Navigation properties
  user?: any;
  sendingUser?: any;
  ccUser?: any;
  notificationFlag?: NotificationFlag;
}

export interface NotificationFlag {
  id: number;
  flag: string;
  backColor: string;
  isActive: boolean;
  canGlow: boolean;
  createdDate: Date | string;
}

export enum TransactionItemType {
  Undefined = 'Undefined',
  Employee = 'Employee',
  Inquiry = 'Inquiry',
  SaleOrder = 'SaleOrder'
}

export interface NotificationCounts {
  unreadCount: number;
  pendingCount: number;
  urgentCount: number;
}
