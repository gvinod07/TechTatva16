package com.purlieus.purlieus.activities;

import android.content.Intent;
import android.content.SharedPreferences;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.view.ViewPager;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.RelativeLayout;
import android.widget.Toast;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.adapters.ProfilePagerAdapter;
import com.purlieus.purlieus.application.Purlieus;
import com.purlieus.purlieus.custom.CustomPager;
import com.purlieus.purlieus.fragments.DetailsFragment;
import com.purlieus.purlieus.fragments.DisplayDetailsFragment;
import com.purlieus.purlieus.fragments.DonateFragment;

public class ProfileActivity extends AppCompatActivity{

    private final long ID_DETAILS_INPUT=0;
    private final long ID_DETAILS_DISPLAY=1;
    private CustomPager pager;
    private FloatingActionButton fab;
    private ActionBar actionBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);
        actionBar = getSupportActionBar();

        if (getSupportActionBar()!=null) {
            getSupportActionBar().setTitle(R.string.profile);
            getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        }

        fab = (FloatingActionButton)findViewById(R.id.fab);
        pager = (CustomPager)findViewById(R.id.profile_view_pager);

        final RelativeLayout parentLayout = (RelativeLayout)findViewById(R.id.activity_profile);

        final DetailsFragment detailsFragment = new DetailsFragment();
        final DisplayDetailsFragment displayDetailsFragment = new DisplayDetailsFragment();

        ProfilePagerAdapter adapter = new ProfilePagerAdapter(getSupportFragmentManager());
        adapter.addFragment(detailsFragment, ID_DETAILS_INPUT);
        adapter.addFragment(displayDetailsFragment, ID_DETAILS_DISPLAY);
        pager.setAdapter(adapter);

        final Button button = (Button)findViewById(R.id.save_button);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (detailsFragment.storeDetails()) {
                    displayDetailsFragment.setData();
                    pager.setCurrentItem(1);
                    button.setVisibility(View.GONE);
                    fab.setVisibility(View.VISIBLE);
                }
                else{
                    Toast.makeText(ProfileActivity.this, "Please fill all details!", Toast.LENGTH_SHORT).show();
                }
            }
        });

        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                pager.setCurrentItem(0);
                fab.setVisibility(View.GONE);
                button.setVisibility(View.VISIBLE);
            }
        });

        SharedPreferences sp = getSharedPreferences(Purlieus.PROFILE_DATA, MODE_PRIVATE);
        if(!sp.getString("name", "").isEmpty() && !sp.getString("bg","").isEmpty() && !sp.getString("address", "").isEmpty()){
            pager.setCurrentItem(1);
            fab.setVisibility(View.VISIBLE);
            button.setVisibility(View.GONE);
        }
        else{
            pager.setCurrentItem(0);
            fab.setVisibility(View.GONE);
            button.setVisibility(View.VISIBLE);
        }


        /**
         *  Button continueButton = (Button) findViewById(R.id.ContinueButton);
        continueButton.setOnClickListener(new View.OnClickListener(){
            @Override
            public void onClick(View v){

            }
        });*/
    }


}
