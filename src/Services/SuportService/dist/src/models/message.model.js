"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const mongoose_1 = require("mongoose");
const messageSchema = new mongoose_1.Schema({
    text: {
        type: String,
    },
    userId: {
        type: String,
    },
}, {
    timestamps: true,
});
exports.Message = mongoose_1.model('Message', messageSchema);
//# sourceMappingURL=message.model.js.map