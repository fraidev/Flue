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
// import bodyParser from 'body-parser'
// import compression from 'compression'
// import errorHandler from 'errorhandler'
// import helmet from 'helmet'
// import morgan from 'morgan';
// require('dotenv-safe').config();
class Server {
    constructor() {
        this.express = express_1.default();
        this.middlewares();
        this.database();
        this.routes();
    }
    middlewares() {
        // this.express.use(helmet());
        // this.express.use(compression());
        // this.express.use(errorHandler());
        this.express.use(body_parser_1.default.json());
        this.express.use(body_parser_1.default.urlencoded({ extended: true }));
        if (process.env.NODE_ENV === 'development') {
            this.express.use(cors_1.default());
            // this.express.use(morgan('tiny'));
        }
    }
    database() {
        mongoose_1.default.connect(`mongodb://localhost:27017/support`, { useNewUrlParser: true }, (err) => {
            if (!err) {
                console.log('Successfully connected to db');
            }
            else {
                console.error(err);
            }
        });
    }
    routes() {
        this.express.use('/', controllers_1.default.supportController);
    }
}
exports.default = new Server().express;
//# sourceMappingURL=server.js.map