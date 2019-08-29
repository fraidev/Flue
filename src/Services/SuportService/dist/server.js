"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const express_1 = __importDefault(require("express"));
const server = express_1.default();
server.get("/", (_, res) => {
    res.send("Hello ts-node!");
});
exports.default = server;
//# sourceMappingURL=server.js.map