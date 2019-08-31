"use strict";Object.defineProperty(exports, "__esModule", {value: true});var _mongoose = require('mongoose');




const messageSchema = new (0, _mongoose.Schema)({
  text: {
    type: String,
  },
  userId: {
    type: String,
  },
}, {
  timestamps: true,
});

 const Message = _mongoose.model('Message', messageSchema); exports.Message = Message;
