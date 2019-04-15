import React from 'react';
import { createAppContainer, createStackNavigator, createSwitchNavigator } from 'react-navigation';

import AuthLoadingScreen from '../screen/auth/SignInScreen';
import AppTabNavigator from './AppTabNavigator';
import SignInScreen from '../screen/auth/SignInScreen';

const AuthStack = createStackNavigator({ SignIn: SignInScreen });

export default createAppContainer(createSwitchNavigator({
  // You could add another route here for authentication.
  // Read more at https://reactnavigation.org/docs/en/auth-flow.html
  AuthLoading: AuthLoadingScreen,
  Auth: AuthStack,
  App: AppTabNavigator,
},
{
  initialRouteName: 'AuthLoading',
}

));