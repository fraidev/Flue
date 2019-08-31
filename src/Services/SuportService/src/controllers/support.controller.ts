import express from 'express';
import { Message } from '../models/message.model';
const router = express.Router();

router.get('/all', async (req, res): Promise<void> => {
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

router.post('/', async (req, res): Promise<void> => {
  try {
    const { text, userId } = req.body;
    const message = new Message();
    message.text = text;
    message.userId = userId;
    message.save();

    res.json({
      message: 'Successfully saved user',
      status: 200,
    });

  } catch (error) {
    res.status(error.status).json(error);
  }
});

export default router;
