import { Document, model, Model, Schema } from 'mongoose';
import { IMessageInterface } from '../types/message';

interface IMessageModel extends IMessageInterface, Document {}

const messageSchema = new Schema({
  text: {
    type: String,
  },
  userId: {
    type: String,
  },
}, {
  timestamps: true,
});

export const Message: Model<IMessageModel> = model<IMessageModel>('Message', messageSchema);
