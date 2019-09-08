"use strict"; function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }Object.defineProperty(exports, "__esModule", {value: true});var _bodyparser = require('body-parser'); var _bodyparser2 = _interopRequireDefault(_bodyparser);
var _cors = require('cors'); var _cors2 = _interopRequireDefault(_cors);
var _express = require('express'); var _express2 = _interopRequireDefault(_express);
var _mongoose = require('mongoose'); var _mongoose2 = _interopRequireDefault(_mongoose);
var _controllers = require('./controllers'); var _controllers2 = _interopRequireDefault(_controllers);

class Server {
  

   constructor() {
    this.express = _express2.default.call(void 0, );

    this.middlewares();
    this.database();
    this.routes();
  }

   middlewares() {
    this.express.use(_bodyparser2.default.json());
    this.express.use(_bodyparser2.default.urlencoded({ extended: true }));
    this.express.use(_cors2.default.call(void 0, ));
  }

   database() {
    _mongoose2.default.connect(
       `mongodb://flue-support:pqcjaCmNfatenQtuHUcsUWCsK7tzNpB3e5S18Xow7eepu30zkA2GNDqN5kzYDzI5BNeFMS9BVUNAmw88y9wdpA%3D%3D@flue-support.documents.azure.com:10255/{flue-support}?ssl=true`,
      // `mongodb://localhost:27017/support`,
      { useNewUrlParser: true },
      (err) => {
        if (!err) {
          console.log('Successfully connected to db');
        } else { console.error(err); }
      },
    );
  }

   routes() {
    this.express.use('/api', _controllers2.default.supportController);
  }
}

exports. default = new Server().express;
