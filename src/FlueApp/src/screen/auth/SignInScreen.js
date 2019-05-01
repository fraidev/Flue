import React from 'react';
import {
  AsyncStorage,
  Button,
  Text,
  TextInput,
  View,
  StyleSheet
} from 'react-native';

export default class SignInScreen extends React.Component {
  static navigationOptions = {
    title: 'Please sign in',
  };

  render() {
    return (
      <View style={styles.container}>
        <Text>Login </Text> 
        <TextInput></TextInput >
        <Text>Senha</Text> 
        <TextInput secureTextEntry={true} />
        <Button title="Entre!" onPress={this._signInAsync} />
        <Button title="Registre!" onPress={this._signInAsync} />
      </View>
    );
  }

  _signInAsync = async () => {
    await AsyncStorage.setItem('userToken', 'abc');
    this.props.navigation.navigate('App');
  };
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    paddingTop: 350,
    backgroundColor: '#fff',
  },
  
});