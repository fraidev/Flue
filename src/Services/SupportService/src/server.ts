import bodyParser from 'body-parser';
import cors from 'cors';
import express from 'express';
import mongoose from 'mongoose';
import controllers from './controllers';

class Server {
  public express: express.Application;

  public constructor() {
    this.express = express();

    this.middlewares();
    this.database();
    this.routes();
  }

  private middlewares(): void {
    this.express.use(bodyParser.json());
    this.express.use(bodyParser.urlencoded({ extended: true }));
    this.express.use(cors());
  }

  private database(): void {
    mongoose.connect(
      `KEEP_YOUR_MONGO_HOST_HERE`,
      { useNewUrlParser: true },
      (err): void => {
        if (!err) {
          console.log('Successfully connected to db');
        } else { console.error(err); }
      },
    );
  }

  private routes(): void {
    this.express.use('/api', controllers.supportController);
  }
}

export default new Server().express;
