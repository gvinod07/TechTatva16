package com.purlieus.purlieus.activities;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.NavigationView;
import android.support.v4.view.MenuItemCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.LinearLayout;

import com.microsoft.windowsazure.mobileservices.*;
import com.microsoft.windowsazure.mobileservices.http.ServiceFilterResponse;
import com.microsoft.windowsazure.mobileservices.table.TableOperationCallback;
import com.purlieus.purlieus.R;
import com.purlieus.purlieus.models.TodoItem;

import java.net.MalformedURLException;

public class MainActivity extends AppCompatActivity {

    private MobileServiceClient mClient;
    private NavigationView navigationView;
    private DrawerLayout drawerLayout;
    private ActionBarDrawerToggle mDrawerToggle;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        setSupportActionBar(toolbar);
        if (getSupportActionBar()!=null) {
            getSupportActionBar().setTitle(R.string.app_name);
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        }

        navigationView = (NavigationView)findViewById(R.id.navigation_drawer);
        navigationView.getMenu().findItem(R.id.nav_menu_categories).setChecked(true);
        drawerLayout = (DrawerLayout)findViewById(R.id.main_drawer_layout);

        View contentMain = findViewById(R.id.main_content);
        LinearLayout health = (LinearLayout)contentMain.findViewById(R.id.health_linear_layout);

        health.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(MainActivity.this, BloodDonationActivity.class);
                startActivity(intent);
            }
        });


        try{
            mClient = new MobileServiceClient("https://purlieus.azurewebsites.net", this);
        }
        catch(MalformedURLException e){
            e.printStackTrace();
        }

        TodoItem item = new TodoItem();
        item.Text="Second item";
        mClient.getTable(TodoItem.class).insert(item, new TableOperationCallback<TodoItem>() {
            @Override
            public void onCompleted(TodoItem entity, Exception exception, ServiceFilterResponse response) {
                if (exception == null) {
                    // Insert succeeded
                    Log.d("Seek", "successful main");
                } else {
                    exception.printStackTrace();
                    Log.d("Seek", "exception in main");
                }
            }
        });

        mDrawerToggle = new ActionBarDrawerToggle(this, drawerLayout, R.string.drawer_open, R.string.drawer_close){
            @Override
            public void onDrawerOpened(View drawerView) {
                invalidateOptionsMenu();
            }

            @Override
            public void onDrawerClosed(View drawerView) {
                invalidateOptionsMenu();
            }
        };
        drawerLayout.addDrawerListener(mDrawerToggle);
        drawerLayout.post(new Runnable() {
            @Override
            public void run() {
                mDrawerToggle.syncState();
            }
        });
        mDrawerToggle.setDrawerIndicatorEnabled(true);
        mDrawerToggle.syncState();

        navigationView.setNavigationItemSelectedListener(new NavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem item) {

                switch(item.getItemId()){
                    case R.id.nav_menu_profile:{
                        Intent intent = new Intent(MainActivity.this, ProfileActivity.class);
                        startActivity(intent);
                    }
                }
                return true;
            }
        });
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if(mDrawerToggle.onOptionsItemSelected(item)){
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
}
