"use strict"; function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }Object.defineProperty(exports, "__esModule", {value: true});var _express = require('express'); var _express2 = _interopRequireDefault(_express);
var _messagemodel = require('../models/message.model');
const router = _express2.default.Router();

router.get('/api/supportMessages', async (req, res) => {
  try {
    const messages = await _messagemodel.Message.find();
    res.json(messages);

  } catch (err) {
    res.status(err.status).json({
      message: err.message,
      status: err.status,
    });
  }
});

router.post('/api/createSuportMessage', async (req, res) => {
  try {
    const { text, userId } = req.body;
    const message = new (0, _messagemodel.Message)();
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

exports. default = router;
