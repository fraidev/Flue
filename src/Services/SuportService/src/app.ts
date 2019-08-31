import './models';
import server from './server';

server.listen(3001, () => {
    console.log(`[SERVER] Running at http://localhost:3001`);
});
