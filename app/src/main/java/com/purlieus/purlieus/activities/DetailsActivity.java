package com.purlieus.purlieus.activities;

import android.app.FragmentTransaction;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.fragments.DetailsFragment;

import static com.purlieus.purlieus.R.id.fab;

/**
 * Created by Shaurya on 07-Oct-16.
 */

public class DetailsActivity extends AppCompatActivity{

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_details);

        setTheme(R.style.AboutTheme);

        final DetailsFragment detailsFragment = new DetailsFragment();

        android.support.v4.app.FragmentTransaction ft = getSupportFragmentManager().beginTransaction().add(R.id.details_fragment_container, detailsFragment);
        ft.commit();

        final Button button = (Button)findViewById(R.id.save_details_button);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (detailsFragment.storeDetails()) {
                    Intent intent = new Intent(DetailsActivity.this, MainActivity.class);
                    startActivity(intent);
                    finish();
                }
                else{
                    Toast.makeText(DetailsActivity.this, "Please fill all details!", Toast.LENGTH_SHORT).show();
                }
            }
        });
    }


}
