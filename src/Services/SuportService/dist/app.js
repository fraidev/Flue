"use strict"; function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }require('./models');
var _server = require('./server'); var _server2 = _interopRequireDefault(_server);

_server2.default.listen(3001, () => {
    console.log(`[SERVER] Running at http://localhost:3001`);
});
