import express from 'express';
import { Message } from '../models/message.model';
const router = express.Router();

router.get('/api/supportMessages', async (req, res): Promise<void> => {
  try {
    const messages = await Message.find();
    res.json(messages);

  } catch (err) {
    res.status(err.status).json({
      message: err.message,
      status: err.status,
    });
  }
});

router.post('/api/createSuportMessage', async (req, res): Promise<void> => {
  try {
    const { text, userId } = req.body;
    const message = new Message();
    message.text = text;
    message.userId = userId;
    message.save();

    res.json({
      message: 'Successfully saved suport message',
      status: 200,
    });

  } catch (error) {
    res.status(error.status).json(error);
  }
});

export default router;
