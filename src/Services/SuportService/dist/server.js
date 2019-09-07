"use strict"; function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }Object.defineProperty(exports, "__esModule", {value: true});var _bodyparser = require('body-parser'); var _bodyparser2 = _interopRequireDefault(_bodyparser);
var _cors = require('cors'); var _cors2 = _interopRequireDefault(_cors);
var _express = require('express'); var _express2 = _interopRequireDefault(_express);
var _mongoose = require('mongoose'); var _mongoose2 = _interopRequireDefault(_mongoose);
var _controllers = require('./controllers'); var _controllers2 = _interopRequireDefault(_controllers);

// import bodyParser from 'body-parser'
// import compression from 'compression'
// import errorHandler from 'errorhandler'
// import helmet from 'helmet'
// import morgan from 'morgan';
// require('dotenv-safe').config();

class Server {
  

   constructor() {
    this.express = _express2.default.call(void 0, );

    this.middlewares();
    this.database();
    this.routes();
  }

   middlewares() {
    // this.express.use(helmet());
    // this.express.use(compression());
    // this.express.use(errorHandler());
    this.express.use(_bodyparser2.default.json());
    this.express.use(_bodyparser2.default.urlencoded({ extended: true }));

    // if (process.env.NODE_ENV === 'development') {
    this.express.use(_cors2.default.call(void 0, ));
      // this.express.use(morgan('tiny'));
    // }
  }

   database() {
    _mongoose2.default.connect(
      `mongodb://localhost:27017/support`,
      { useNewUrlParser: true },
      (err) => {
        if (!err) {
          console.log('Successfully connected to db');
        } else { console.error(err); }
      },
    );
  }

   routes() {
    this.express.use('/', _controllers2.default.supportController);
  }
}

exports. default = new Server().express;
