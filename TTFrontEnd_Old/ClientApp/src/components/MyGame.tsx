import * as React from 'react';
import { connect } from 'react-redux';
//import * as MyGameStore from '../store/MyGame';
import * as MyGameStore from '../store/MyGame';
import { ApplicationState } from '../store';
import { RouteComponentProps } from 'react-router';
import { Box, Typography, Container, Grid, CssBaseline, Paper, FormControlLabel, Checkbox, TextField, Button, CircularProgress } from '@material-ui/core'
import { Link, useHistory, useLocation } from 'react-router-dom'

type MyGamePros =
    MyGameStore.MyGameState &
    typeof MyGameStore.actionCreators &
    RouteComponentProps<{}>;
    
    

function MyGame(props: MyGamePros){

    const handleSubmit = (e: any) => {
        e.preventDefault();
        //setLoading(true);

        //your client side validation here

        //after success validation
        //const userData = {
        //    email: values.email,
        //    password: values.password,
        //};

        //props.loginUser(userData, props.history);
    }

    const handleChange = (e: any) => {
        //e.persist();
        //setValues(values => ({ ...values, [e.target.name]: e.target.value }));
    };
       
        return (
            <Box>
                <Box >
                    <Typography variant="h4">
                        <Box fontWeight={600} letterSpacing={3}>
                            SIGN IN
                    </Box>
                    </Typography>
                </Box>
                <Container
                    component="main"
                    maxWidth="md">
                    <CssBaseline />
                    <Grid
                        container
                        alignContent="center"
                        alignItems="center"
                        justify="center"
                        spacing={3}>
                        <Grid
                            item
                            md={3}>
                            <img alt="My Logo" />
                        </Grid>
                        <Grid item md={9}>
                            <Paper>
                                <Box>
                                    <TextField
                                        variant="outlined"
                                        margin="none"

                                        fullWidth
                                        id="email"
                                        label="Email Address"
                                        name="email"
                                        type="email"
                                        onChange={handleChange}
                                    />
                                    <TextField
                                        variant="outlined"
                                        margin="normal"
                                        value=""
                                        fullWidth
                                        name="password"
                                        label="Password"
                                        type="password"
                                        onChange={handleChange}
                                    />
                                    <Grid container>
                                        <Grid item sm={6} md={6}>
                                            <FormControlLabel

                                                control={
                                                    <Checkbox value="remember" color="primary" />
                                                }
                                                label="Remember me"
                                            />
                                        </Grid>
                                        <Grid item sm={6} md={6}>
                                            <Link to="#">
                                                Forgot password?
                                            </Link>
                                        </Grid>
                                    </Grid>
                                    <Button type="submit" variant="contained" color="primary">
                                        Login
                                    </Button>

                                </Box>
                            </Paper>
                        </Grid>
                    </Grid>
                </Container >
            </Box>
        );
};
export default connect(
    (state: ApplicationState) => state.mygamecounter,
    MyGameStore.actionCreators
)(MyGame);

