import './models';
import server from './server';

server.listen(5006, () => {
    console.log(`[SERVER] Running at http://localhost:5006`);
});
