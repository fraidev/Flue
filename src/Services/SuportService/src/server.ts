import bodyParser from 'body-parser';
import cors from 'cors';
import express from 'express';
import mongoose from 'mongoose';
import controllers from './controllers';

// import bodyParser from 'body-parser'
// import compression from 'compression'
// import errorHandler from 'errorhandler'
// import helmet from 'helmet'
// import morgan from 'morgan';
// require('dotenv-safe').config();

class Server {
  public express: express.Application;

  public constructor() {
    this.express = express();

    this.middlewares();
    this.database();
    this.routes();
  }

  private middlewares(): void {
    // this.express.use(helmet());
    // this.express.use(compression());
    // this.express.use(errorHandler());
    this.express.use(bodyParser.json());
    this.express.use(bodyParser.urlencoded({ extended: true }));

    if (process.env.NODE_ENV === 'development') {
      this.express.use(cors());
      // this.express.use(morgan('tiny'));
    }
  }

  private database(): void {
    mongoose.connect(
      `mongodb://localhost:27017/support`,
      { useNewUrlParser: true },
      (err): void => {
        if (!err) {
          console.log('Successfully connected to db');
        } else { console.error(err); }
      },
    );
  }

  private routes(): void {
    this.express.use('/', controllers.supportController);
  }
}

export default new Server().express;
