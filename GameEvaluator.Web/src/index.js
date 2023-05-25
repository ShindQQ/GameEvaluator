import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import { RenderGames } from './Games/RenderGames';
import reportWebVitals from './reportWebVitals';
import store from './store';
import { Provider } from 'react-redux'

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <Provider store={store}>
    <RenderGames />
  </Provider>
);

reportWebVitals();
