import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './components/Home';
import CalendarContainer from './components/CalendarContainer';

export const routes = <Layout>
    <Route exact path='/' component={ Home } />
    <Route path='/calendar/:regionStr/:year' component={ CalendarContainer } />
</Layout>;
