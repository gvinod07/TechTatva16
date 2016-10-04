package com.purlieus.purlieus.activities;

import android.content.Intent;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.fragments.DetailsFragment;

public class ProfileActivity extends AppCompatActivity {


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);

        DetailsFragment fragment = new DetailsFragment();
        android.app.FragmentTransaction transaction = getFragmentManager().beginTransaction().add(R.id.fragmentInputDetails, fragment, "inpur_fragment");
        transaction.commit();
        /**
         *  Button continueButton = (Button) findViewById(R.id.ContinueButton);
        continueButton.setOnClickListener(new View.OnClickListener(){
            @Override
            public void onClick(View v){

            }
        });*/
    }


}
