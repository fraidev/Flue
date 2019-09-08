"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const body_parser_1 = __importDefault(require("body-parser"));
const cors_1 = __importDefault(require("cors"));
const express_1 = __importDefault(require("express"));
const mongoose_1 = __importDefault(require("mongoose"));
const controllers_1 = __importDefault(require("./controllers"));
class Server {
    constructor() {
        this.express = express_1.default();
        this.middlewares();
        this.database();
        this.routes();
    }
    middlewares() {
        this.express.use(body_parser_1.default.json());
        this.express.use(body_parser_1.default.urlencoded({ extended: true }));
        this.express.use(cors_1.default());
    }
    database() {
        mongoose_1.default.connect(`mongodb://flue-support:pqcjaCmNfatenQtuHUcsUWCsK7tzNpB3e5S18Xow7eepu30zkA2GNDqN5kzYDzI5BNeFMS9BVUNAmw88y9wdpA%3D%3D@flue-support.documents.azure.com:10255/{flue-support}?ssl=true`, 
        // `mongodb://localhost:27017/support`,
        { useNewUrlParser: true }, (err) => {
            if (!err) {
                console.log('Successfully connected to db');
            }
            else {
                console.error(err);
            }
        });
    }
    routes() {
        this.express.use('/api', controllers_1.default.supportController);
    }
}
exports.default = new Server().express;
//# sourceMappingURL=server.js.map